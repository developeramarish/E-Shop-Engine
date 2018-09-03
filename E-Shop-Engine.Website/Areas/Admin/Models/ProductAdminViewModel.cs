﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Utilities;

namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class ProductAdminViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(5000)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be higher than 0.")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Number In Stock")]
        [Range(0, int.MaxValue)]
        public int NumberInStock { get; set; }

        [Display(Name = "Catalog Number")]
        [StringLength(100)]
        public string CatalogNumber { get; set; }

        [Display(Name = "Image")]
        public HttpPostedFileBase ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public byte[] ImageBytes { get; set; }

        public string ImageMimeType { get; set; }

        [Display(Name = "Display product at main page as special offer?")]
        public bool ShowAsSpecialOffer { get; set; } = false;

        [Display(Name = "Display product at main page?")]
        public bool ShowAtMainPage { get; set; } = false;

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        [Display(Name = "Subcategory")]
        public int? SubcategoryId { get; set; }

        public string SubcategoryName { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Edited { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Subcategory> Subcategories { get; set; }

        public static implicit operator ProductAdminViewModel(Product product)
        {
            return new ProductAdminViewModel
            {
                Id = product.ID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                NumberInStock = product.NumberInStock,
                CatalogNumber = product.CatalogNumber,
                ImageBytes = product.ImageData,
                ImageMimeType = product.ImageMimeType,
                ShowAsSpecialOffer = product.ShowAsSpecialOffer,
                ShowAtMainPage = product.ShowAtMainPage,
                CategoryId = product.CategoryID,
                SubcategoryId = product.SubcategoryID,
                Created = product.Created,
                Edited = product.Edited,
                CategoryName = product.Category.Name,
                SubcategoryName = product.Subcategory?.Name
            };
        }

        public static implicit operator Product(ProductAdminViewModel viewModel)
        {
            byte[] imageData = null;

            if (viewModel.ImageBytes == null || viewModel.ImageData != null)
            {
                imageData = ConvertPostedFile.ToByteArray(viewModel.ImageData);
            }
            else
            {
                imageData = viewModel.ImageBytes;
            }

            if (viewModel.ImageMimeType == null || (viewModel.ImageMimeType != null && viewModel.ImageData != null && viewModel.ImageData?.ContentType != viewModel.ImageMimeType))
            {
                viewModel.ImageMimeType = viewModel.ImageData?.ContentType;
            }

            return new Product
            {
                ID = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                CatalogNumber = viewModel.CatalogNumber,
                CategoryID = viewModel.CategoryId,
                SubcategoryID = viewModel.SubcategoryId,
                ImageData = imageData,
                ImageMimeType = viewModel.ImageMimeType,
                NumberInStock = viewModel.NumberInStock,
                Price = viewModel.Price,
                ShowAsSpecialOffer = viewModel.ShowAsSpecialOffer,
                ShowAtMainPage = viewModel.ShowAtMainPage,
                Created = viewModel.Created,
                Edited = viewModel.Edited
            };
        }
    }
}