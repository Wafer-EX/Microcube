using Microcube.Game.Blocks;
using Microcube.Game.Blocks.Moving;
using Microcube.Graphics.ColorModels;
using Silk.NET.Maths;

namespace Microcube.Game
{
    /// <summary>
    /// Represents a level class that include all blocks, move queues, player and etc.
    /// and that's all can be updated by this.
    /// </summary>
    public class Level
    {
        private List<Block>? blocks;

        /// <summary>
        /// This event calls when a prism was collected.
        /// </summary>
        public event Action? PrismCollected;

        /// <summary>
        /// This event calls when the level was finished.
        /// </summary>
        public event Action? Finished;

        /// <summary>
        /// All blocks of this level not include player.
        /// </summary>
        public List<Block> Blocks
        {
            get
            {
                if (blocks == null)
                    throw new NullReferenceException();

                return blocks;
            }
            set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                blocks = value;
                foreach (Block block in blocks)
                {
                    if (block is Prism)
                        PrismCount++;
                }
            }
        }

        /// <summary>
        /// All move queues in the level.
        /// </summary>
        public List<MoveQueue> MoveQueues { get; private set; }

        /// <summary>
        /// Player of this level.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Collected prism count.
        /// </summary>
        public int CollectedPrisms { get; private set; }

        /// <summary>
        /// All prisms that are in the level, include collected.
        /// </summary>
        public int PrismCount { get; private set; }

        /// <summary>
        /// Is this level finished or not.
        /// </summary>
        public bool IsFinished { get; private set; }

        public Level(Block[] blocks, MoveQueue[] moveQueues, Vector3D<float> playerStartPosition)
        {
            ArgumentNullException.ThrowIfNull(blocks, nameof(blocks));
            ArgumentNullException.ThrowIfNull(moveQueues, nameof(moveQueues));

            Blocks = new List<Block>(blocks);
            MoveQueues = new List<MoveQueue>(moveQueues);
            Player = new Player(playerStartPosition, new RgbaColor(1.0f, 0.0f, 0.0f, 1.0f), this);
        }

        /// <summary>
        /// Updates all blocks, move queues and player.
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            if (!IsFinished)
                Player.Update(deltaTime);

            foreach (var moveQueue in MoveQueues)
                moveQueue.Update(deltaTime);

            foreach (var block in Blocks)
            {
                if (block is IDynamic dynamicBlock)
                    dynamicBlock.Update(deltaTime, this);
            }
        }

        /// <summary>
        /// When it's called, collected prism count increases to 1.
        /// </summary>
        public void CollectPrism()
        {
            CollectedPrisms++;
            PrismCollected?.Invoke();
        }

        /// <summary>
        /// When it's called the level sets to finished and can't be controller.
        /// </summary>
        public void Finish()
        {
            if (!IsFinished)
            {
                Finished?.Invoke();
                IsFinished = true;
            }
        }

        public IEnumerable<Vector3D<float>> GetBarrierBlockCoordinates()
        {
            foreach (var block in Blocks)
            {
                if (block.IsBarrier)
                    yield return block.Position;
            }
        }

        /// <summary>
        /// It's used to check is player has something in bottom or not.
        /// </summary>
        /// <param name="posX">Position X to check</param>
        /// <param name="posZ">Position Z to check</param>
        /// <param name="height">Restriction on Y coorditane.</param>
        /// <returns></returns>
        public Block? GetHighestBarrierFromHeight(float posX, float posZ, float height)
        {
            // TODO: refactor?
            Block? highestBlock = null;
            var pointPosition = new Vector2D<float>(posX, posZ);

            foreach (var block in Blocks)
            {
                if (block.IsBarrier && block.Position.Y < height)
                {
                    var blockPosition = new Vector2D<float>(block.Position.X, block.Position.Z);
                    float distance = Vector2D.Distance(pointPosition, blockPosition);

                    if (distance < 1.0f)
                    {
                        if (highestBlock == null || block.Position.Y > highestBlock.Position.Y)
                        {
                            highestBlock = block;
                        }
                    }
                }
            }
            return highestBlock;
        }
    }
}