﻿using System;
using System.Collections.Generic;

namespace BrewIoT.Shared.Models
{
    public sealed class Device
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DeviceType DeviceType { get; set; }
    }

    public enum DeviceType
    {
        Unknown,
        Meadow
    }

    public class Recipe
    {
        public int Id { get; set; }

        public int Version { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<RecipeStep> Steps { get; set; } = new List<RecipeStep>();
    }

    public class RecipeStep
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public double? TargetTemperature { get; set; }
        
        public TimeSpan Duration { get; set; }
    }

// Job - effectively the recipe
    public sealed class Job
    {
        public int Id { get; set; }

        public Device? Device { get; set; }

        public Recipe? Recipe { get; set; }
    }

    public class DeviceReading
    {
        public double LiquidTemperature { get; set; }

        public double AmbientTemperature { get; set; }

        public double TargetTemperature { get; set; }

        public DateTime Timestamp { get; set; }
    }

// JobStage - effectively each step in the recipe
    public sealed class JobStage
    {
        public int Id { get; set; }

        public Job? Job { get; set; }

        public RecipeStep? RecipeStep { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public JobStageStatus Status { get; set; }
    }

    public enum JobStageStatus
    {
        Unknown,
        Pending,
        InProgress,
        Complete,
        Failed
    }
}