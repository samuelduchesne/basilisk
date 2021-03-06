﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Basilisk.Controls.InterfaceModels
{
    public abstract class LayeredConstruction : ConstructionBase
    {
        private ObservableCollection<MaterialLayer> layers = new ObservableCollection<MaterialLayer>();

        public ObservableCollection<MaterialLayer> Layers
        {
            get { return layers; }
            set
            {
                if (layers == value) { return; }
                if (layers != null)
                {
                    foreach (var layer in layers) { layer.PropertyChanged -= OnLayersChanged; }
                    layers.CollectionChanged -= OnLayersChanged;
                }
                layers = value;
                foreach (var layer in layers) { layer.PropertyChanged += OnLayersChanged; }
                layers.CollectionChanged += OnLayersChanged;
                RaisePropertyChanged(this, nameof(Layers));
            }
        }

        public override IEnumerable<LibraryComponent> AllReferencedComponents =>
            Layers
            .Select(layer => layer.Material)
            .Where(mat => mat != null)
            .Distinct();

        public override bool DirectlyReferences(LibraryComponent component) =>
            Layers.Any(layer => layer.Material == component);

        protected void CopyBasePropertiesFrom(LayeredConstruction source)
        {
            var layers =
                source
                .Layers
                .Select(layer => new MaterialLayer()
                {
                    Material = layer.Material,
                    Thickness = layer.Thickness
                });
            Layers = new ObservableCollection<MaterialLayer>(layers);
            base.CopyBasePropertiesFrom(source);
        }

        protected void CopyBasePropertiesFrom(LayeredConstruction source, ComponentCoordinator coord)
        {
            var layers =
                source
                .Layers
                .Select(layer => new MaterialLayer()
                {
                    Material = coord.GetWithSameName(layer.Material),
                    Thickness = layer.Thickness
                });
            Layers = new ObservableCollection<MaterialLayer>(layers);
            base.CopyBasePropertiesFrom(source);
        }

        private void OnLayersChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(sender, nameof(Layers));
        }
    }
}
