var Dolittle = Dolittle || {};
(function(global, undefined) {
    Dolittle.extend = function extend(destination, source) {
        return $.extend(destination, source);
    };
})(window);