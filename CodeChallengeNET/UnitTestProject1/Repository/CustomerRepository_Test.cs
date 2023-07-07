using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1.Repository
{
    /// <summary>
    /// Descripción resumida de CustomerRepository_Test
    /// </summary>
    [TestClass]
    public class CustomerRepository_Test
    {
        private const string BaseUrl = "http://localhost:5000"; // Replace with your API base URL

        public CustomerRepository_Test()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Obtiene o establece el contexto de las pruebas que proporciona
        ///información y funcionalidad para la serie de pruebas actual.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Atributos de prueba adicionales
        //
        // Puede usar los siguientes atributos adicionales conforme escribe las pruebas:
        //
        // Use ClassInitialize para ejecutar el código antes de ejecutar la primera prueba en la clase
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup para ejecutar el código una vez ejecutadas todas las pruebas en una clase
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Usar TestInitialize para ejecutar el código antes de ejecutar cada prueba 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup para ejecutar el código una vez ejecutadas todas las pruebas
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void InsertCustomer_ShouldReturnSuccess()
        {
            //
            // TODO: Agregar aquí la lógica de las pruebas
            //
        }
        /* TODO
        [TestMethod]
        public void GetCustomers_ShouldReturnListOfCustomerViewModels()
        {
            // Arrange
            var repo = new CustomerRepository(); 

            // Act
            var result = await repo.GetCustomers();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ResponseViewModel<List<CustomerViewModel>>));
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count > 0);
        }
        */
    }
}
