Dolittle.namespace("Dolittle");
Dolittle.Exception.define("Dolittle.LocationNotSpecified","Location was not specified");
Dolittle.Exception.define("Dolittle.InvalidUriFormat", "Uri format specified is not valid");
Dolittle.Exception.define("Dolittle.ObjectLiteralNotAllowed", "Object literal is not allowed");
Dolittle.Exception.define("Dolittle.MissingTypeDefinition", "Type definition was not specified");
Dolittle.Exception.define("Dolittle.AsynchronousDependenciesDetected", "You should consider using Type.beginCreate() or dependencyResolver.beginResolve() for systems that has asynchronous dependencies");
Dolittle.Exception.define("Dolittle.UnresolvedDependencies", "Some dependencies was not possible to resolve");