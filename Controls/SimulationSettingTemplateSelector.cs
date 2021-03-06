﻿using System.Windows;
using System.Windows.Controls;

namespace Basilisk.Controls
{
    public class SimulationSettingTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var grid = container as FrameworkElement;
            var setting = item as SimulationSetting;
            if (grid != null && setting != null)
            {
                var template =
                    setting.ShowMultivalueDescription ? grid.FindResource("MultiValueDescriptionTemplate") :
                    setting.SettingType == SettingType.Enum ? grid.FindResource("EnumPropertyTemplate") :
                    setting.SettingType == SettingType.Bool ? grid.FindResource("BoolPropertyTemplate") :
                    setting.SettingType == SettingType.Reference ? grid.FindResource("ReferencePropertyTemplate") :
                    grid.FindResource("TextPropertyTemplate");
                return (DataTemplate)template;
            }
            return null;
        }
    }
}
