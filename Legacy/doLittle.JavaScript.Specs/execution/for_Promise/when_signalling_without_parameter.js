describe("when signalling without parameter", function () {
    var promise = Dolittle.execution.Promise.create();
    var nextPromise;

    promise.continueWith(function (p) {
        nextPromise = p;
    });

    promise.signal();

    it("should pass along next promise", function () {
        expect(nextPromise instanceof Dolittle.execution.Promise).toBe(true);
    });
});