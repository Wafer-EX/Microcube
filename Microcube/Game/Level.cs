using Microcube.Game.Blocks;
using Microcube.Game.Blocks.Moving;
using Microcube.Graphics.ColorModels;
using Silk.NET.Maths;

namespace Microcube.Game
{
    public class Level
    {
        private List<Block>? blocks;

        public event Action? PrismCollected;
        public event Action? Finished;

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

        public List<MoveQueue> MoveQueues { get; private set; }

        public Player Player { get; private set; }

        public int CollectedPrisms { get; private set; }

        public int PrismCount { get; private set; }

        public bool IsFinished { get; private set; }

        public Level(Block[] blocks, MoveQueue[] moveQueues, Vector3D<float> playerStartPosition)
        {
            ArgumentNullException.ThrowIfNull(blocks, nameof(blocks));
            ArgumentNullException.ThrowIfNull(moveQueues, nameof(moveQueues));

            Blocks = new List<Block>(blocks);
            MoveQueues = new List<MoveQueue>(moveQueues);
            Player = new Player(playerStartPosition, new RgbaColor(1.0f, 0.0f, 0.0f, 1.0f), this);
        }

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

        // TODO: remove this method?
        public void CollectPrism()
        {
            CollectedPrisms++;
            PrismCollected?.Invoke();
        }

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