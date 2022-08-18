using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


namespace ROS2
{
public class motorControl : MonoBehaviour
{
    private ROS2UnityComponent ros2Unity;
    private ROS2Node ros2Node;
    private ISubscription<std_msgs.msg.Float32> sub;
    
    public ArticulationBody articulationBodie;
    public string topic;
    public string node;

    SynchronizationContext context;


    // Start is called before the first frame update
    void Start()
    {
        ros2Unity = GetComponent<ROS2UnityComponent>();
        context = SynchronizationContext.Current;
    }

    // Update is called once per frame
    void Update()
    {
        if (ros2Node == null && ros2Unity.Ok())
        {
            ros2Node = ros2Unity.CreateNode(node);

            sub = ros2Node.CreateSubscription<std_msgs.msg.Float32>(
              topic, msg =>motor(msg.Data));
            
        }
    }

    void motor(float pwr){
        context.Post(_ =>{
            var drive = this.articulationBodie.xDrive;
            drive.targetVelocity = pwr * 500;
            this.articulationBodie.xDrive = drive;
        },null);
    }
}

}
