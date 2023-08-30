# Bulwark.Auth.Admin

Bulwark.Auth.Admin is a add-on for Bulwark.Auth which is JWT token authentication and account management service that easily fits into any
infrastructure.

Bulwark admin adds user management and administration to Bulwark.Auth.

Note: this service should not be exposed to the public internet. It is 
used as internal tool with a lot of power. 

# Releases and contributing guidelines

### Releases

Pre-releases will be developed on the `beta` branches. Once the beta is stable it will be merged into `main` and a release will be generated

The docker tags for `beta` will be in this format: `1.0.11-beta.x`
The docker tags for `production` releases will be in this format: `1.0.11`

Most work for an upcoming release will do pull request against the `beta` branch.

Depending on the change, for example important security fixes or bug fixes; it will be decided
to have a PR on `main` or will hold off the PR on `beta` branch until the next release.

If it is important bug fix or security update a release will happen ASAP.

A releases will be targeted monthly, but if important feature(s) that are scheduled for the next upcoming release are not complete it is possible to push the release to the next release cycle.
If there is no features schedule for a release it will be skipped.


### Contributions

- Each contribution will need a issue/ticket created to track the feature or bug fix before it will be considered
- The PR must pass all tests and be reviewed by an official maintainer
- Each PR must be linked to an issue/ticket, once the PR is merged the ticket will be auto closed
- Each feature/bugfix needs to have unit tests
- Each feature must have the code documented inline 
