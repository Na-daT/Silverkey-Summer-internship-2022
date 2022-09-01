﻿//------------------------------------------------------------------------------
// <auto-generated>This code was generated by LLBLGen Pro v5.9.</auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;

namespace recipesApp.EntityClasses
{
	/// <summary>Class which represents the entity 'Ingredient'.</summary>
	public partial class Ingredient : CommonEntityBase
	{
		/// <summary>Method called from the constructor</summary>
		partial void OnCreated();
		private System.Int64 _id = default(System.Int64);

		/// <summary>Initializes a new instance of the <see cref="Ingredient"/> class.</summary>
		public Ingredient() : base()
		{
			this.Name = string.Empty;
			OnCreated();
		}

		/// <summary>Gets the Id field. </summary>
		public System.Int64 Id => _id;
		/// <summary>Gets or sets the Name field. </summary>
		public System.String Name { get; set; }
		/// <summary>Gets or sets the RecipeId field. </summary>
		public System.Int64 RecipeId { get; set; }
		/// <summary>Represents the navigator which is mapped onto the association 'Ingredient.Recipe - Recipe.Ingredients (m:1)'</summary>
		public virtual Recipe Recipe { get; set; } = null!;
	}
}
