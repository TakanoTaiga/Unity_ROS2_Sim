using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ROS2{
public class LiDAR : MonoBehaviour
{
    private ROS2UnityComponent ros2unity;
    private ROS2Node ros2node;
    private IPublisher<sensor_msgs.msg.LaserScan> pub;


    private float maxDistance  = 4.095f;
    private float minDistance = 0.06f;
    private float angle_max = 300f;
    private float angle_min = 60f;
    private int laser_volume = 682;


    private float angle_increment;
    void Start()
    {
        angle_max = angle_max * (Mathf.PI / 180);
        angle_min = angle_min * (Mathf.PI / 180);
        angle_increment = (angle_max - angle_min) / (float)(laser_volume);
        ros2unity = GetComponent<ROS2UnityComponent>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        float[] distance = new float[laser_volume];
        for(int i = 0; i < laser_volume ; i++){
            float angle_rad = angle_increment * (float)i + angle_min - this.transform.localEulerAngles.y * (Mathf.PI / 180);

            Vector3 angle = new Vector3(Mathf.Cos(angle_rad) , 0 , Mathf.Sin(angle_rad));
            RaycastHit hit;
            
            if(Physics.Raycast(transform.position , angle , out hit,maxDistance)){
                if(hit.distance > minDistance){
                    distance[i] = hit.distance + Random.Range(-1f * hit.distance * 0.01f , hit.distance * 0.01f);
                    Debug.DrawRay(transform.position , angle * distance[i], Color.green , 0.01f , false);
                }else{
                    distance[i] = maxDistance;
                    Debug.DrawRay(transform.position , angle * distance[i], Color.red , 0.01f , false);
                }
            }else{
                distance[i] = maxDistance;
                Debug.DrawRay(transform.position , angle * distance[i], Color.red , 0.01f , false);
            }

            
        }

        if(ros2unity.Ok()){
            if(ros2node == null){
                ros2node = ros2unity.CreateNode("UnityLiDAR");
                pub = ros2node.CreatePublisher<sensor_msgs.msg.LaserScan>("laser");
            }
            sensor_msgs.msg.LaserScan msg = new sensor_msgs.msg.LaserScan();
            msg.Header.Frame_id = "laser";
            msg.Angle_min = angle_min;
            msg.Angle_max = angle_max;
            msg.Angle_increment = angle_increment;
            msg.Range_min = minDistance;
            msg.Range_max = maxDistance;
            msg.Ranges = distance;
            pub.Publish(msg);
        }
    }
}

}
