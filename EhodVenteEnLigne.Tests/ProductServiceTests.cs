using EhodBoutiqueEnLigne.Models;
using EhodBoutiqueEnLigne.Models.Repositories;
using EhodBoutiqueEnLigne.Models.Services;
using EhodBoutiqueEnLigne.Models.ViewModels;
using Microsoft.Extensions.Localization;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace EhodBoutiqueEnLigne.Tests
{
    public class TestsDeServiceDeProduit
    {
        private readonly Mock<ICart> _mockPourPanier;
        private readonly Mock<IProductRepository> _mockPourRepositoryProduit;
        private readonly Mock<IOrderRepository> _mockPourRepositoryCommande;
        private readonly Mock<IStringLocalizer<ProductService>> _mockPourLocalisateur;
        private readonly ProductService _serviceProduit;

        public TestsDeServiceDeProduit()
        {
            _mockPourLocalisateur = new Mock<IStringLocalizer<ProductService>>();
            _mockPourLocalisateur.Setup(l => l["MissingName"]).Returns(new LocalizedString("MissingName", "MissingName"));
            _mockPourLocalisateur.Setup(l => l["MissingQuantity"]).Returns(new LocalizedString("MissingQuantity", "MissingQuantity"));
            _mockPourLocalisateur.Setup(l => l["MissingPrice"]).Returns(new LocalizedString("MissingPrice", "MissingPrice"));
            _mockPourLocalisateur.Setup(l => l["PriceNotANumber"]).Returns(new LocalizedString("PriceNotANumber", "PriceNotANumber"));
            _mockPourLocalisateur.Setup(l => l["PriceNotGreaterThanZero"]).Returns(new LocalizedString("PriceNotGreaterThanZero", "PriceNotGreaterThanZero"));
            _mockPourLocalisateur.Setup(l => l["StockNotGreaterThanZero"]).Returns(new LocalizedString("StockNotGreaterThanZero", "StockNotGreaterThanZero"));
            _mockPourLocalisateur.Setup(l => l["StockNotAnInteger"]).Returns(new LocalizedString("StockNotAnInteger", "StockNotAnInteger"));
            _mockPourPanier = new Mock<ICart>();
            _mockPourRepositoryProduit = new Mock<IProductRepository>();
            _mockPourRepositoryCommande = new Mock<IOrderRepository>();
            _serviceProduit = new ProductService(_mockPourPanier.Object, _mockPourRepositoryProduit.Object, _mockPourRepositoryCommande.Object, _mockPourLocalisateur.Object);
        }

        [Fact]
        public void EnregistrerProduit_TousLesChampsRemplis_NeDoitPasRetournerMessagesErreur()
        {
            // Arrange
            var produitViewModel = new ProductViewModel
            {
                Name = "cable",
                Description = "cable marque apple",
                Price = "2000",
                Stock = "8"
            };

            // Act
            _serviceProduit.SaveProduct(produitViewModel);
            var erreursModele = _serviceProduit.CheckProductModelErrors(produitViewModel);

            // Assert
            Assert.Empty(erreursModele);
        }

        [Fact]
        public void EnregistrerProduit_MissingName_DevraitRetournerMessageErreur()
        {
            // Arrange
            var produitViewModel = new ProductViewModel
            {
                Name = " ",
                Description = "Description du produit",
                Price = "1000",
                Stock = "7"
            };

            // Act
            _serviceProduit.SaveProduct(produitViewModel);
            var erreursModele = _serviceProduit.CheckProductModelErrors(produitViewModel);

            // Assert
            Assert.Contains("MissingName", erreursModele);
        }

        [Fact]
        public void EnregistrerProduit_StockVide_DevraitRetournerMessageErreur()
        {
            // Arrange
            var produitViewModel = new ProductViewModel
            {
                Name = "iphone 5",
                Description = "Test Description",
                Price = "70000",
                Stock = "3"
            };

            // Act
            _serviceProduit.SaveProduct(produitViewModel);
            var erreursModele = _serviceProduit.CheckProductModelErrors(produitViewModel);

            // Assert
            Assert.Contains("MissingQuantity", erreursModele);
        }

        [Fact]
        public void EnregistrerProduit_StockNonEntier_DevraitRetournerMessageErreur()
        {
            // Arrange
            var produitViewModel = new ProductViewModel
            {
                Name = "casque",
                Description = "casque apple",
                Price = "200000",
                Stock = "3,1"
            };

            // Act
            _serviceProduit.SaveProduct(produitViewModel);
            var erreursModele = _serviceProduit.CheckProductModelErrors(produitViewModel);

            // Assert
            Assert.Contains("StockNotAnInteger", erreursModele);
        }

        [Fact]
        public void EnregistrerProduit_StockInferieurAZero_DevraitRetournerMessageErreur()
        {
            // Arrange
            var produitViewModel = new ProductViewModel
            {
                Name = "iphone 5",
                Description = "iphhone 5",
                Price = "60000",
                Stock = "-5"
            };

            // Act
            _serviceProduit.SaveProduct(produitViewModel);
            var erreursModele = _serviceProduit.CheckProductModelErrors(produitViewModel);

            // Assert
            Assert.Contains("StockNotGreaterThanZero", erreursModele);
        }

        [Fact]
        public void EnregistrerProduit_MissingPrice_DevraitRetournerMessageErreur()
        {
            // Arrange
            var produitViewModel = new ProductViewModel
            {
                Name = "iphone 6",
                Description = "iphone 6",
                Price = " ",
                Stock = "9"
            };

            // Act
            _serviceProduit.SaveProduct(produitViewModel);
            var erreursModele = _serviceProduit.CheckProductModelErrors(produitViewModel);

            // Assert
            Assert.Contains("MissingPrice", erreursModele);
        }

        [Fact]
        public void EnregistrerProduit_PriceNonNumerique_DevraitRetournerMessageErreur()
        {
            // Arrange
            var produitViewModel = new ProductViewModel
            {
                Name = "article",
                Description = "article avec mauvais prix",
                Price = "zzzz",
                Stock = "4"
            };

            // Act
            _serviceProduit.SaveProduct(produitViewModel);
            var erreursModele = _serviceProduit.CheckProductModelErrors(produitViewModel);

            // Assert
            Assert.Contains("PriceNotANumber", erreursModele);
        }

        [Fact]
        public void EnregistrerProduit_PriceInferieurAZero_DevraitRetournerMessageErreur()
        {
            // Arrange
            var produitViewModel = new ProductViewModel
            {
                Name = "article",
                Description = "article avec  prix null",
                Price = "0",
                Stock = "1"
            };

            // Act
            _serviceProduit.SaveProduct(produitViewModel);
            var erreursModele = _serviceProduit.CheckProductModelErrors(produitViewModel);

            // Assert
            Assert.Contains("PriceNotGreaterThanZero", erreursModele);
        }
    }
}
