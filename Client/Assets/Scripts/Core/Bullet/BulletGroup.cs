/*
 * @Author: fasthro
 * @Date: 2018-12-29 14:15:54
 * @Description: 子弹组
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    [System.Serializable]
    public class BulletGroup
    {
        // 子弹资源路径
        public string bulletAssetPath;
		// 子弹速度
		public float speed;
		// 子弹生命时间
		public float lifeTime;
        // 子弹数量
        public int count;
        // 每个子弹之间的时间间隔
        public float delay;
        // 没个子弹相对目标点的角度偏移
        public Vector3[] angles;

        public void Attack(Vector3 position, Quaternion rotation, AttackData attackData, ActorBase target)
        {
			Game.mainGame.StartCoroutine(Fire(position, rotation, attackData, target));
        }

        IEnumerator Fire(Vector3 position, Quaternion rotation, AttackData attackData, ActorBase target)
        {
            for (int i = 0; i < count; i++)
            {
                CreateBullet(i, position, rotation, attackData, target);

                if (delay > 0)
                {
					yield return new WaitForSeconds(delay);
                }
            }
            yield return null;
        }

        public void CreateBullet(int index, Vector3 position, Quaternion rotation, AttackData attackData, ActorBase target)
        {
            var bullet = Game.gamePool.Spawn<GeneralBullet>(bulletAssetPath);
            bullet.Initialize();
			bullet.speed = speed;
			bullet.lifeTime = lifeTime;
            bullet.firePosition = position;
            bullet.fireRotation = Quaternion.Euler(rotation.eulerAngles + angles[index]);
            bullet.Spawn(attackData, target);
        }
    }
}
