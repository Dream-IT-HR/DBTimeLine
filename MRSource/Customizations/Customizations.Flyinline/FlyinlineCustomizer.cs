using Customizations.Core.Attributes;
using Framework.DBTimeLine;
using Framework.DBTimeLine.DBObjects;
using System;
using System.Collections.Generic;

namespace Customizations.Flyinline
{
    [Customization(CustomizationKey = "FlyinlineERM")]
    public class FlyinlineCustomizer : Core.Customizer
    {
        [MethodActivationCustomization(ActivationKey = "CreateTimeLine")]
        public void CreateTimeLine(Dictionary<string, object> inputs)
        {
            DBTimeLiner dbTimeLiner = (DBTimeLiner)inputs["DBTimeLiner"];

            var moduleIdentity = new FlyinlineModule();
            
            moduleIdentity.Parent = dbTimeLiner;

            dbTimeLiner.DBModules.Add(moduleIdentity);

            moduleIdentity.CreateTimeLine();          
        }
    }
}
