angular.module("wscApp")
    .controller('CartCtrl', function ($scope, srvCart) {

        $scope.subtotals = srvCart.subtotals;
        
        $scope.addToCart = srvCart.add;

        $scope.removeFromCart = srvCart.remove;

});