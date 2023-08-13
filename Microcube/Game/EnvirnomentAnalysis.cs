using Microcube.Game.Blocks.Enums;
using Silk.NET.Maths;

namespace Microcube.Game
{
    public static class EnvirnomentAnalysis
    {
        public static PlayerBarrier GetSingleBarrierFromPosition(Vector3D<float> blockPosition, Vector3D<float> barrierPosition, bool isReversed, bool changeAxis)
        {
            float blockAxis = barrierPosition.Z;
            float playerAxis = blockPosition.Z;
            float blockDifferentAxis = barrierPosition.X;
            float playerDifferentAxis = blockPosition.X;

            if (changeAxis)
            {
                (blockAxis, blockDifferentAxis) = (blockDifferentAxis, blockAxis);
                (playerAxis, playerDifferentAxis) = (playerDifferentAxis, playerAxis);
            }

            float planeDistance = (MathF.Max(playerAxis, blockAxis) - MathF.Min(blockAxis, playerAxis)) * (blockAxis < playerAxis ? -1.0f : 1.0f);
            float planeDifferentAxisDistance = (MathF.Max(playerDifferentAxis, blockDifferentAxis) - MathF.Min(blockDifferentAxis, playerDifferentAxis)) * (blockDifferentAxis < playerDifferentAxis ? -1.0f : 1.0f);

            float heightDistance = barrierPosition.Y - blockPosition.Y;

            if (MathF.Abs(planeDifferentAxisDistance) < 1.0f)
            {
                if (MathF.Abs(planeDistance) < 1.0f && heightDistance == 1.0f)
                    return PlayerBarrier.Trap;

                if ((!isReversed && planeDistance > 0.0f || isReversed && planeDistance < 0.0f) && MathF.Abs(planeDistance) > 1.0f && MathF.Abs(planeDistance) < 2.0f && MathF.Abs(heightDistance) < 1.0f)
                    return PlayerBarrier.Unsuitable;
            }

            if ((isReversed && planeDistance == -1.0f) || (!isReversed && planeDistance == 1.0f))
            {
                if (MathF.Abs(blockDifferentAxis - playerDifferentAxis) < 1.0f)
                {
                    return heightDistance switch
                    {
                        0.0f => PlayerBarrier.Step,
                        > 0.0f and <= 1.0f => PlayerBarrier.Wall,
                        < 0.0f and > -1.0f => PlayerBarrier.Unsuitable,
                        _ => PlayerBarrier.Nothing
                    };
                }
            }

            return PlayerBarrier.Nothing;
        }

        public static PlayerBarrier GetGlobalBarrierFromPosition(Vector3D<float> blockPosition, IEnumerable<Vector3D<float>> barrierPositions, bool isReversed, bool changeAxis)
        {
            PlayerBarrier finalforwardBarrier = PlayerBarrier.Nothing;
            PlayerBarrier finalbackBarrier = PlayerBarrier.Nothing;

            foreach (var barrierPosition in barrierPositions)
            {
                PlayerBarrier forwardBarrier = GetSingleBarrierFromPosition(blockPosition, barrierPosition, isReversed, changeAxis);
                PlayerBarrier backBarrier = GetSingleBarrierFromPosition(blockPosition, barrierPosition, !isReversed, changeAxis);

                if (forwardBarrier > finalforwardBarrier)
                    finalforwardBarrier = forwardBarrier;

                if (backBarrier > finalbackBarrier)
                    finalbackBarrier = backBarrier;

                // TODO: improve this condition?
                if (finalforwardBarrier == PlayerBarrier.Step && barrierPosition == blockPosition + new Vector3D<float>(0.0f, 2.0f, 0.0f))
                    finalforwardBarrier = PlayerBarrier.Wall;
            }

            if ((finalforwardBarrier == PlayerBarrier.Step || finalforwardBarrier == PlayerBarrier.Wall) && (finalbackBarrier == PlayerBarrier.Step || finalbackBarrier == PlayerBarrier.Wall))
                return PlayerBarrier.Trap;

            return finalforwardBarrier;
        }
    }
}