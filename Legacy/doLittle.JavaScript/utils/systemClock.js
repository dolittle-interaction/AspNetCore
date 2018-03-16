Dolittle.namespace("Dolittle",{
    systemClock: Dolittle.Singleton(function () {
        this.nowInMilliseconds = function () {
            return window.performance.now();
        };
    })
});