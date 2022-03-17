using Godot;
using System.Collections.Generic;

public class WaveManager : Node2D
{
    PackedScene BasicEnemyPackScene;
    SceneNode MainSceneNode;

    private int CurrentWave = 1;

    private int WaveStartTime;
    private float CurrentWaveTime;
    private int WaveMileStone = 2;
    private bool WaveIsActive = false;

    private int BaseSpawnAmount;
    private int SpawnAmount;
    private int EnemyIncrementAmount;
    private List<BasicEnemy> SpawnedEnemies = new List<BasicEnemy>();

    private int UpdateEveryTimer = 1;
    private float CurrentUpdateTime = 0;

    private float BaseEnemyHPModifier;
    private float BaseEnemySpeedModifier;
    private float BaseEnemyDamageModifier;
    private float EnemyHPModifier;
    private float EnemyHPModifierIncrement;
    private float EnemySpeedModifier;
    private float EnemySpeedModifierIncrement;
    private float EnemyDamageModifier;
    private float EnemyDamageModifierIncrement;
    public override void _Ready()
    {
        MainSceneNode = GetParent() as SceneNode;
        BasicEnemyPackScene = (PackedScene)ResourceLoader.Load("res://Scenes/Enemies/BasicEnemy.tscn");
        switch(MainSceneNode.WorldDifficulty)
        {
            case Difficulty.Easy:
                WaveStartTime = 90;
                BaseSpawnAmount = 1;
                SpawnAmount = BaseSpawnAmount;
                EnemyIncrementAmount = 2;

                BaseEnemyHPModifier = 0.8F;
                EnemyHPModifier = BaseEnemyHPModifier;
                EnemyHPModifierIncrement = 0.1F;

                BaseEnemySpeedModifier = 0.8F;
                EnemySpeedModifier = BaseEnemySpeedModifier;
                EnemySpeedModifierIncrement = 0.05F;

                BaseEnemyDamageModifier = 0.5F;
                EnemyDamageModifier = BaseEnemyDamageModifier;
                EnemyDamageModifierIncrement = 0.2F;
                break;
            case Difficulty.Normal:
                WaveStartTime = 75;
                BaseSpawnAmount = 2;
                SpawnAmount = BaseSpawnAmount;
                EnemyIncrementAmount = 2;

                BaseEnemyHPModifier = 1F;
                EnemyHPModifier = BaseEnemyHPModifier;
                EnemyHPModifierIncrement = 0.15F;

                BaseEnemySpeedModifier = 1F;
                EnemySpeedModifier = BaseEnemySpeedModifier;
                EnemySpeedModifierIncrement = 0.1F;

                BaseEnemyDamageModifier = 0.7F;
                EnemyDamageModifier = BaseEnemyDamageModifier;
                EnemyDamageModifierIncrement = 0.25F;
                break;
            case Difficulty.Hard:
                WaveStartTime = 60;
                BaseSpawnAmount = 2;
                SpawnAmount = BaseSpawnAmount;
                EnemyIncrementAmount = 3;

                BaseEnemyHPModifier = 1.2F;
                EnemyHPModifier = BaseEnemyHPModifier;
                EnemyHPModifierIncrement = 0.2F;

                BaseEnemySpeedModifier = 1F;
                EnemySpeedModifier = BaseEnemySpeedModifier;
                EnemySpeedModifierIncrement = 0.15F;

                BaseEnemyDamageModifier = 0.5F;
                EnemyDamageModifier = BaseEnemyDamageModifier;
                EnemyDamageModifierIncrement = 0.3F;
                break;
        }
        CurrentWaveTime = WaveStartTime;
    }

    public override void _PhysicsProcess(float delta)
    {
/*        CurrentUpdateTime += delta;
        if (!WaveIsActive) CurrentWaveTime -= delta;
        if (CurrentWaveTime <= 0 && !WaveIsActive)
        {
            StartWave();
        }

        if (CurrentUpdateTime >= UpdateEveryTimer)
        {
            HUDManager.WaveTimerHUD.UpdateTime((int)CurrentWaveTime);
            CurrentUpdateTime = 0;
            if (WaveIsActive)
            {
                SpawnEnemy();
            }
        }*/
    }

    private void StartWave()
    {
        WaveIsActive = true;
        CurrentWave++;
    }

    public void OnEnemyDestroy(BasicEnemy basicEnemy)
    {
        SpawnedEnemies.Remove(basicEnemy);
        if (SpawnedEnemies.Count == 0)
        {
            WaveIsActive = false;
            CurrentWaveTime = WaveStartTime;
            if (CurrentWave % WaveMileStone == 0)
            {
                UpdateEnemiesModifiers();
                SpawnAmount += EnemyIncrementAmount;
            }
        }
    }

    private void UpdateEnemiesModifiers()
    {
        EnemyDamageModifier += EnemyDamageModifierIncrement;
        EnemyHPModifier += EnemyHPModifierIncrement;
        EnemySpeedModifier += EnemySpeedModifierIncrement;
    }
    private void SpawnEnemy()
    {
        if (SpawnedEnemies.Count >= SpawnAmount) return;
        SpawnedEnemies.ForEach(x =>
        {
            if (x.GlobalPosition == GlobalPosition) return;
        });
        BasicEnemy SpawnedEnemy = (BasicEnemy)BasicEnemyPackScene.Instance();
        SpawnedEnemy.GlobalPosition = GlobalPosition;
        SpawnedEnemy.Health = (int)(SpawnedEnemy.BaseMaxHealth * EnemyHPModifier);
        SpawnedEnemy.Damage = (int)(SpawnedEnemy.BaseDamage * EnemyDamageModifier);
        SpawnedEnemy.Speed = (int)(SpawnedEnemy.BaseSpeed * EnemySpeedModifier);

        SpawnedEnemies.Add(SpawnedEnemy);
        MainSceneNode.CallDeferred("add_child", SpawnedEnemy);
    }

}
