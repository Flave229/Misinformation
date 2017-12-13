using UnityEngine;

namespace Assets.Scripts.Physics
{
    class CollisionBox : MonoBehaviour
    {
        public CollisionBoxType CollisionBoxType;

        public CollisionBox()
        {

        }

        public bool CheckCollisions(Sprite thisSprite, Transform thisTransform)
        {
            var collisionObjects = (CollisionBox[])UnityEngine.Object.FindObjectsOfType(typeof(CollisionBox));

            foreach (CollisionBox potentialCollider in collisionObjects)
            {
                if (potentialCollider == this)
                    continue;

                if (CheckCollidesWith(thisSprite, thisTransform, potentialCollider))
                    return true;
            }

            return false;
        }

        private bool CheckCollidesWith(Sprite thisSprite, Transform thisTransform, CollisionBox collider)
        {
            if (CollisionBoxType == CollisionBoxType.RECTANGLE && collider.CollisionBoxType == CollisionBoxType.RECTANGLE)
                return AABBCollision(thisSprite, thisTransform, collider);

            return false;
        }

        private bool AABBCollision(Sprite thisSprite, Transform thisTransform, CollisionBox collider)
        {
            var colliderSprite = collider.GetComponent<SpriteRenderer>().sprite;

            if (thisTransform.position.x + thisSprite.bounds.min.x < collider.transform.position.x + colliderSprite.bounds.max.x
                && thisTransform.position.x + thisSprite.bounds.max.x > collider.transform.position.x + colliderSprite.bounds.min.x
                && thisTransform.position.y + thisSprite.bounds.min.y < collider.transform.position.y + colliderSprite.bounds.max.y
                && thisTransform.position.y + thisSprite.bounds.max.y > collider.transform.position.y + colliderSprite.bounds.min.y)
                return true;

            return false;
        }

        public static bool PointInBoxCollision(Vector2 collider1Position, Vector2 collider1Size, Vector2 collider2Point)
        {
            if (collider1Position.x - (collider1Size.x / 2) < collider2Point.x
                && collider1Position.x + (collider1Size.x / 2) > collider2Point.x
                && collider1Position.y - (collider1Size.y / 2) < collider2Point.y
                && collider1Position.y + (collider1Size.y / 2) > collider2Point.y)
                return true;

            return false;
        }
    }
}
