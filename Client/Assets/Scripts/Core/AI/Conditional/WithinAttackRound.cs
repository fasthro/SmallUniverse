using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using SmallUniverse.Utils;

namespace SmallUniverse.AI
{
    [TaskCategory("Custom/WithinAttack")]
    [TaskDescription("判断目标是否在攻击范围内-攻击范围以自身为中心点的圆形范围内")]
    public class WithinAttackRound : Conditional
    {
        [Tooltip("目标 Transform")]
        public SharedTransform target;
        [Tooltip("攻击范围半径")]
        public SharedFloat radius;

        public override TaskStatus OnUpdate()
        {
            if(ShapeUtils.WithinRound(transform.position, target.Value.position, radius.Value))
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
