describe("when handling with one element", function() {
	var root = document.createElement("div");
	var element = document.createElement("button");
	
	var documentService = {
		traverseObjects: function(callback) {
			callback(element);
		}
	};

	var visitStub = sinon.stub();

	var visitorType = Dolittle.markup.ElementVisitor.extend(function() {
		this.visit = visitStub;
	});
	var actions = { some: "actions" };

	beforeEach(function() {
	    sinon.stub(Dolittle.markup.ElementVisitor, "getExtenders").returns([visitorType]);
	    sinon.stub(Dolittle.markup.ElementVisitorResultActions, "create").returns(actions);

	    var instance = Dolittle.views.UIManager.createWithoutScope({
			documentService: documentService
		})

		instance.handle(root);
	});

	afterEach(function() {
	    Dolittle.markup.ElementVisitor.getExtenders.restore();
	    Dolittle.markup.ElementVisitorResultActions.create.restore();
	});

	it("should call the visit function of the visitor", function() {
		expect(visitStub.calledWith(element, actions)).toBe(true);
	});
});