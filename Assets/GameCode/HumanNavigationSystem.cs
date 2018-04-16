﻿using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms2D;

class HumanNavigationSystem : JobComponentSystem
{
    public struct HumanData
    {
        public int Length;
        public ComponentDataArray<Position2D> Position;
        public ComponentDataArray<Heading2D> Heading;
        public ComponentDataArray<Human> Human;
    }

    [Inject] private HumanData humanDatum;


    struct HumanNavigationJob : IJobParallelFor
    {
        public HumanData humanDatum;
        public void Execute(int index)
        {
            Heading2D heading2D = humanDatum.Heading[index];
            heading2D.Value = new Unity.Mathematics.float2(1f, 0f);
            humanDatum.Heading[index] = heading2D;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var humanNavigationJob = new HumanNavigationJob
        {
            humanDatum = humanDatum
        };

        return humanNavigationJob.Schedule(humanDatum.Length, 64, inputDeps);
    }
}
