﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Basilisk.Controls.Attributes;

namespace Basilisk.Controls.InterfaceModels
{
    [UseDefaultValuesOf(typeof(Core.ZoneConstructions))]
    [DisplayName("zone constructions")]
    [ComponentNamespace]
    public class ZoneConstructions : LibraryComponent
    {
        [SimulationSetting]
        public OpaqueConstruction Facade { get; set; }

        [SimulationSetting]
        public OpaqueConstruction Ground { get; set; }

        [SimulationSetting]
        public OpaqueConstruction Partition { get; set; }

        [SimulationSetting]
        public OpaqueConstruction Roof { get; set; }

        [SimulationSetting]
        public OpaqueConstruction Slab { get; set; }

        [SimulationSetting(DisplayName = "Facade is adiabatic")]
        public bool IsFacadeAdiabatic { get; set; }

        [SimulationSetting(DisplayName = "Ground is adiabatic")]
        public bool IsGroundAdiabatic { get; set; }

        [SimulationSetting(DisplayName = "Partition is adiabatic")]
        public bool IsPartitionAdiabatic { get; set; }

        [SimulationSetting(DisplayName = "Roof is adiabatic")]
        public bool IsRoofAdiabatic { get; set; }

        [SimulationSetting(DisplayName = "Slab is adiabatic")]
        public bool IsSlabAdiabatic { get; set; }

        public override IEnumerable<LibraryComponent> AllReferencedComponents
        {
            get
            {
                var direct = new LibraryComponent[]
                {
                    Facade,
                    Ground,
                    Partition,
                    Roof,
                    Slab
                }.Where(d => d != null);
                return
                    direct
                    .Concat(direct.SelectMany(d => d.AllReferencedComponents))
                    .Distinct();
            }
        }

        public override bool DirectlyReferences(LibraryComponent component) =>
            Facade == component ||
            Ground == component ||
            Partition == component ||
            Roof == component ||
            Slab == component;

        public override LibraryComponent Duplicate()
        {
            var res = new ZoneConstructions()
            {
                Facade = Facade,
                Ground = Ground,
                Partition = Partition,
                Roof = Roof,
                Slab = Slab,
                IsFacadeAdiabatic = IsFacadeAdiabatic,
                IsGroundAdiabatic = IsGroundAdiabatic,
                IsPartitionAdiabatic = IsPartitionAdiabatic,
                IsRoofAdiabatic = IsRoofAdiabatic,
                IsSlabAdiabatic = IsSlabAdiabatic
            };
            res.CopyBasePropertiesFrom(this);
            return res;
        }

        public override void OverwriteWith(LibraryComponent other, ComponentCoordinator coord)
        {
            var c = (ZoneConstructions)other;
            Facade = coord.GetWithSameName(c.Facade);
            Ground = coord.GetWithSameName(c.Ground);
            Roof = coord.GetWithSameName(c.Roof);
            Slab = coord.GetWithSameName(c.Slab);
            Partition = coord.GetWithSameName(c.Partition);
            IsFacadeAdiabatic = c.IsFacadeAdiabatic;
            IsGroundAdiabatic = c.IsGroundAdiabatic;
            IsPartitionAdiabatic = c.IsPartitionAdiabatic;
            IsRoofAdiabatic = c.IsRoofAdiabatic;
            IsSlabAdiabatic = c.IsSlabAdiabatic;
            CopyBasePropertiesFrom(c);
        }
    }
}
