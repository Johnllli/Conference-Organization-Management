﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 2025/4/4 20:52:00
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace COM
{
    public partial class Presenter {

        public Presenter()
        {
            this.Presentations1 = new List<Presentation>();
            OnCreated();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string FieldOfExpertise { get; set; }

        public virtual IList<Presentation> Presentations1 { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
