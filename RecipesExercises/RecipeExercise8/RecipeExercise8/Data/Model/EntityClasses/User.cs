﻿//------------------------------------------------------------------------------
// <auto-generated>This code was generated by LLBLGen Pro v5.9.</auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System;
using System.Collections.Generic;

namespace RecipesApp.EntityClasses
{
	/// <summary>Class which represents the entity 'User'.</summary>
	public partial class User : CommonEntityBase
	{
		/// <summary>Method called from the constructor</summary>
		partial void OnCreated();
		private System.Int32 _id = default(System.Int32);

		/// <summary>Initializes a new instance of the <see cref="User"/> class.</summary>
		public User() : base()
		{
			this.Password = string.Empty;
			this.Username = string.Empty;
			OnCreated();
		}

		/// <summary>Gets the Id field. </summary>
		public System.Int32 Id => _id;
		/// <summary>Gets or sets the IsActive field. </summary>
		public System.Boolean IsActive { get; set; }
		/// <summary>Gets or sets the Password field. </summary>
		public System.String Password { get; set; }
		/// <summary>Gets or sets the RefreshToken field. </summary>
		public System.String? RefreshToken { get; set; }
		/// <summary>Gets or sets the RefreshTokenExpiry field. </summary>
		public Nullable<System.DateTime> RefreshTokenExpiry { get; set; }
		/// <summary>Gets or sets the Username field. </summary>
		public System.String Username { get; set; }
	}
}