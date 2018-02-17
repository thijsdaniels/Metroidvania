using Objects.Obstacles;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    public class Explodable : Damagable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cause"></param>
        /// <returns></returns>
        protected override bool Immune(Damager cause)
        {
            return !cause.GetComponent<Explosion>();
        }
    }
}
