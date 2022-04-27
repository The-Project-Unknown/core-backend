# core-backend

This repository should be used for developing new services.

Rules for services:
  The Services can be written in any language
  The Services must have corresponding Dockerfile available
  The Services must be able to communicate with redis
  Every service must register itself to redis as "ready to consume work" after its started
  The services must be able to run in multiple instances simultaneously, unless otherwise noted

