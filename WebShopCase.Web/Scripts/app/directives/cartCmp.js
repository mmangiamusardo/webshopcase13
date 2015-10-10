angular.module("wscApp")
    .directive("cartComponent", function (srvCart) {
        return {
            restrict: "E",
            templateUrl: "Scripts/app/components/cartCmp.html",
            controller: function ($scope) {

                srvCart.clear();

                $scope.size = srvCart.size;
                $scope.total = srvCart.total;
                $scope.totalVat = srvCart.totalVat;
            }
        };
});