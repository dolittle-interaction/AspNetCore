describe("when creating instance of two definition", function () {
    var firstSingleton = Dolittle.Singleton(function () {
        this.something = "Hello";
    });
    var secondSingleton = Dolittle.Singleton(function () {
        this.something = "World";
    });

    var firstInstance = firstSingleton.create();
    var secondInstance = secondSingleton.create();

    it("should return two different instances", function () {
        expect(firstInstance.something).not.toBe(secondInstance.something);
    });
});