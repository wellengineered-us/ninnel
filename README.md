# WellEngineered :: Ninnel

## Overview

Ninnel is a *serverless syndication platform*. What exactly does that mean?

A syndication platform has three key capabilities:

* Produce a stream of records from a source and consume into a destination, similar to an enterprise application integration system.
* Titrate streams of records in a stateless and ephemeral manner.
* Process streams of records as they appear from the source.

Ninnel is generally used for two broad classes of solutions:

* Building unbuffered, streaming I/O oriented data pipelines that efficiently promotes data between disparate hosting zones.
* Building self-hosted, embedded pipeline components that transform or react to streams of data.

To understand how Ninnel does these things, let's dive in and explore Ninnel's capabilities from the bottom up.

First a few concepts:

* Ninnel is run as a pipeline in the form of a task on a single compute container.
* The Ninnel pipeline bounds a stream of records within a channel.
* Each record consists in totality of an index, topic, schema, payload, instant, partition, offset, and headers.

Ninnel has an abstraction API which consists of the following extensibility points:

* Connectors - enables building and running reusable source and/or destination adapters connecting a variety of systems and services using pull-iterator semantics.
* Processors - enables a pipeline to act as a stream processor, applying transformation logic in a deferred-execution manner.
* Runtime - enables advanced customization of the core platform plumbing.
* Configuration - establishes a rich in-memory and over-the-wire configuration model.

In Ninnel, there is no concept of communication between any 'clients' or 'servers' as the grain of distribution is more "compact".

Distributed streaming platforms are designed to "widen naturally" at cloud scale - this is due to their distributed, horizontally elastic architecture.
Centralized integration platforms are typically designed to "mushroom awkwardly" at enterprise scale - this is due to their centralized, vertically constrained architecture with horizontal elasticity as an afterthought.
While these approaches have their place in a variety of solutions, Ninnel was designed to be intentionally different - while using familiar software and distributed systems patterns.

To use an analogy:

*IF*

* Kafka cloud-scale elasticity is to the *observable universe* in classical physics

*THEN*

* Ninnel compute-scale resiliency is to the *Planck length* in quantum physics. 

##### Putting the Pieces Together

Ninnel is designed to operate at commodity appliance scale (e.g. Raspberry Pi) with full fidelity over cloud-native serverless compute containers (e.g. AWS Lambda). 
Ninnel, with its unique architecture, makes it well-suited for on-premises to cloud data synchronization use cases where IT friction is contraindicated (e.g. 'cloud-to-ground' branch office topologies).

##### What's the Catch?

There is none. Ninnel is a fully functional, extensible, open source software project and is also an integral component of the overall several commercial ecosystems.

# Development

To build a development snapshot of Ninnel, you can use either:

* Visual Studio, VS Code, Rider, or your favorite .NET Core IDE
* MS Build via standard build targets

# FAQ

Refer frequently asked questions on SyncPrem Ninnel here:

* Wiki: https://github.com/wellengineered-us/ninnel/wiki/FAQ

# Contribute

- Source Code: https://github.com/wellengineered-us/ninnel
- Issue Tracker: https://github.com/wellengineered-us/ninnel/issues

# License

The project is licensed under the MIT License.


# License

The project is licensed under the MIT License.
