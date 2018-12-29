# RestFullAppInteraction
Client side desktop WPF application to interact with Web Server Restfull service.
It has no database, instead it uses "mobile.json" file in local directory to maintain data operations.
To run this application correctly you must change port number in url string, example:
private static string url = "http://localhost:57447/api/RestAPI";
My port number here is 57447, you must change this to your own.
And you must run "WebApplicationTest1" application from "RestFulInteractionWebApp" repository on IIS
