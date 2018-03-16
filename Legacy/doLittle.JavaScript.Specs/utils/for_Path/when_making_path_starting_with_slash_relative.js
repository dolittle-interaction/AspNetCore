describe("when making path starting with slash relative", function () {
    var relative = Dolittle.Path.makeRelative("/absolute/path.js");

    it("should removing slash", function () {
        expect(relative).toBe("absolute/path.js");
    });
});