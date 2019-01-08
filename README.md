# Symbotic test project

This repo should contain implementation of the test task:
"Generator of random calls to external services"

From the rational point this should be a service that performs generator driven testing on different kinds of external APIs.

Here *Test* is a single bunch of work requested by the *client*: it has specific *density* (or *number*) of calls [to external API by specific *transport*] with specific *probability distribution* along the timeline of a given *duration*.

## User stories

- As a client I want system to execute only single test at the moment of time in order to prevent concurrency and it is just a business need

- As a client I want to request any amount of tests at any given point of time to be executed once the latest test is finished in order to delay subsequent tests execution while satisfying the story above

- As a client I want to configure each test in the request body: the endpoint, transport, probability distribution, [and all properties of a test]; in order to run any kind of a test I want

- As a client I want service to support transport HTTP, accept HTTP call parameters in the request body (HTTP METHOD, custom HTTP headers) and allow templating of messages bodies in *appsettings.json* [N templates per specific endpoint] in order to send random HTTP POSTs to external APIs

- As a product owner I want to be able to support any other transport technology in the future

- As a client I want to subscribe to test start/completion in order to receive statistics of resulting HTTP statuses

_TBD_

## Non-func requirements

- Format of data contracts is your choice

- Use .Net Core SDK 2.1/2.2

- Use RabbitMQ to receive client requests, publish updates

- As a transport technology demonstrate only HTTP

- Service should be easily replicated to N instances to parallelize a work

- Create basic CI pipeline in *Travis CI* to build Docker image (Alpine linux)
    - don't need to push image to registry
    - any scripting tools suitable for you

- Add essential unit tests

- Provide the integration test that demonstrates all the work (do not run it on CI)
    - scenario should not use mocks, should use real transport and messaging

- Provide tutorial how to run the app as a Docker image with custom templates for HTTP calls

- Logging and error handling

- This should not be a single commit, structure your way of thinking: align to user stories or your own breakdown of the work

# Submit the results

1. Fork this repo to work at the beginning

2. Create Pull Request with results and send it back