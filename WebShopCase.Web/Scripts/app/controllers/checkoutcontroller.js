angular.module("wscApp")
    .controller('CheckoutCtrl', function ($scope, $location, srvSharedData, srvCart, srvAccount, toaster) {

        if (!srvAccount.isUserAuthenticated()) {

            toaster.pop({
                type: 'error',
                body: "User must be logged in to checkout",
                bodyOutputType: 'trustedHtml',
            });

            $location.path("/signin");
        } 

        $scope.subtotals = srvCart.subtotals();

        // to fill by service
        $scope.shippers =
        [
            { ShipperID: 1, CompanyName: "Shipper1"},
            { ShipperID: 2, CompanyName: "Shipper2" },
            { ShipperID: 3, CompanyName: "Shipper3" },
            { ShipperID: 4, CompanyName: "Shipper4" }
        ];
        $scope.selectedShipper = $scope.shippers[3].ShipperID;

        $scope.checkout = function () {

            var orderDetails = new Array();

            for (var i in $scope.subtotals) {
                orderDetails.push(
                    {
                        ProductID: $scope.subtotals[i].productId,
                        UnitPrice: $scope.subtotals[i].productPrice,
                        Quantity:  $scope.subtotals[i].totalQty
                    });
            }

            var Order = {};
            Order.OrderDetails = orderDetails;
            Order.ShipName = $scope.customer.FirstName + ' ' + $scope.customer.LastName;
            Order.ShipAddress = $scope.customer.Address;
            Order.ShipCity = $scope.customer.City;
            Order.ShipPostalCode = $scope.customer.PostalCode;
            Order.ShipCountry = $scope.customer.Country;
            Order.CustomerID = srvSharedData.getCustomer().CustomerID;
            Order.ShipperID = Number($scope.selectedShipper);

            srvSharedData.setOrder(Order);

            $location.path('/process');
        };
    });
