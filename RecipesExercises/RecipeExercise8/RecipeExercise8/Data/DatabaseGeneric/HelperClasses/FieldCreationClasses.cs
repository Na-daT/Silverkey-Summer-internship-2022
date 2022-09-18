﻿//////////////////////////////////////////////////////////////
// <auto-generated>This code was generated by LLBLGen Pro 5.9.</auto-generated>
//////////////////////////////////////////////////////////////
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
//////////////////////////////////////////////////////////////
using System;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace recipeDatabase.HelperClasses
{
	/// <summary>Field Creation Class for entity CategoryEntity</summary>
	public partial class CategoryFields
	{
		/// <summary>Creates a new CategoryEntity.Id field instance</summary>
		public static EntityField2 Id { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(CategoryFieldIndex.Id); }}
		/// <summary>Creates a new CategoryEntity.IsActive field instance</summary>
		public static EntityField2 IsActive { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(CategoryFieldIndex.IsActive); }}
		/// <summary>Creates a new CategoryEntity.Name field instance</summary>
		public static EntityField2 Name { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(CategoryFieldIndex.Name); }}
	}

	/// <summary>Field Creation Class for entity IngredientEntity</summary>
	public partial class IngredientFields
	{
		/// <summary>Creates a new IngredientEntity.Id field instance</summary>
		public static EntityField2 Id { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(IngredientFieldIndex.Id); }}
		/// <summary>Creates a new IngredientEntity.IsActive field instance</summary>
		public static EntityField2 IsActive { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(IngredientFieldIndex.IsActive); }}
		/// <summary>Creates a new IngredientEntity.Name field instance</summary>
		public static EntityField2 Name { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(IngredientFieldIndex.Name); }}
		/// <summary>Creates a new IngredientEntity.RecipeId field instance</summary>
		public static EntityField2 RecipeId { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(IngredientFieldIndex.RecipeId); }}
	}

	/// <summary>Field Creation Class for entity InstructionEntity</summary>
	public partial class InstructionFields
	{
		/// <summary>Creates a new InstructionEntity.Id field instance</summary>
		public static EntityField2 Id { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(InstructionFieldIndex.Id); }}
		/// <summary>Creates a new InstructionEntity.IsActive field instance</summary>
		public static EntityField2 IsActive { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(InstructionFieldIndex.IsActive); }}
		/// <summary>Creates a new InstructionEntity.Name field instance</summary>
		public static EntityField2 Name { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(InstructionFieldIndex.Name); }}
		/// <summary>Creates a new InstructionEntity.RecipeId field instance</summary>
		public static EntityField2 RecipeId { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(InstructionFieldIndex.RecipeId); }}
	}

	/// <summary>Field Creation Class for entity RecipeEntity</summary>
	public partial class RecipeFields
	{
		/// <summary>Creates a new RecipeEntity.Id field instance</summary>
		public static EntityField2 Id { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(RecipeFieldIndex.Id); }}
		/// <summary>Creates a new RecipeEntity.IsActive field instance</summary>
		public static EntityField2 IsActive { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(RecipeFieldIndex.IsActive); }}
		/// <summary>Creates a new RecipeEntity.Title field instance</summary>
		public static EntityField2 Title { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(RecipeFieldIndex.Title); }}
	}

	/// <summary>Field Creation Class for entity RecipeCategoryEntity</summary>
	public partial class RecipeCategoryFields
	{
		/// <summary>Creates a new RecipeCategoryEntity.CategoryId field instance</summary>
		public static EntityField2 CategoryId { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(RecipeCategoryFieldIndex.CategoryId); }}
		/// <summary>Creates a new RecipeCategoryEntity.Id field instance</summary>
		public static EntityField2 Id { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(RecipeCategoryFieldIndex.Id); }}
		/// <summary>Creates a new RecipeCategoryEntity.IsActive field instance</summary>
		public static EntityField2 IsActive { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(RecipeCategoryFieldIndex.IsActive); }}
		/// <summary>Creates a new RecipeCategoryEntity.RecipeId field instance</summary>
		public static EntityField2 RecipeId { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(RecipeCategoryFieldIndex.RecipeId); }}
	}

	/// <summary>Field Creation Class for entity UserEntity</summary>
	public partial class UserFields
	{
		/// <summary>Creates a new UserEntity.Id field instance</summary>
		public static EntityField2 Id { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(UserFieldIndex.Id); }}
		/// <summary>Creates a new UserEntity.IsActive field instance</summary>
		public static EntityField2 IsActive { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(UserFieldIndex.IsActive); }}
		/// <summary>Creates a new UserEntity.Password field instance</summary>
		public static EntityField2 Password { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(UserFieldIndex.Password); }}
		/// <summary>Creates a new UserEntity.RefreshToken field instance</summary>
		public static EntityField2 RefreshToken { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(UserFieldIndex.RefreshToken); }}
		/// <summary>Creates a new UserEntity.RefreshTokenExpiry field instance</summary>
		public static EntityField2 RefreshTokenExpiry { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(UserFieldIndex.RefreshTokenExpiry); }}
		/// <summary>Creates a new UserEntity.Username field instance</summary>
		public static EntityField2 Username { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(UserFieldIndex.Username); }}
	}

	/// <summary>Field Creation Class for entity VersionInfoEntity</summary>
	public partial class VersionInfoFields
	{
		/// <summary>Creates a new VersionInfoEntity.AppliedOn field instance</summary>
		public static EntityField2 AppliedOn { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(VersionInfoFieldIndex.AppliedOn); }}
		/// <summary>Creates a new VersionInfoEntity.Description field instance</summary>
		public static EntityField2 Description { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(VersionInfoFieldIndex.Description); }}
		/// <summary>Creates a new VersionInfoEntity.Version field instance</summary>
		public static EntityField2 Version { get { return ModelInfoProviderSingleton.GetInstance().CreateField2(VersionInfoFieldIndex.Version); }}
	}
	

}