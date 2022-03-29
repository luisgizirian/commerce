# Commerce

[Español](README.es.md)

**Please note!** The experiments in this repository should be considered to be in beta. Significant portions of these experiments are subject to change without warning (although accesible through repo's history). **No part of this code should be considered stable.**

Managemnt Software for running your daily business operations. Extensible and easy to integrate.

> Please check the [Discussions](https://github.com/luisgizirian/commerce/discussions) area, and join us in there. It's a great way to participate!

### Mongo DB manual DB initialization (until automated comes...)

1. Run the App (it'll create DB + Collection in MongoDB Container). No results rendered.
2. Head your browser to http://localhost:5002
2. Get a Console for the running MongoDB container  with `docker exec -it <CONTAINER> /bin/bash`

```
> mongosh

> use cmmrcdb

> db.Catalog.insertMany([{"Name": "Albahaca", "Sku": "001", "IsEnabled": true}, {"Name": "Alcachofa", "Sku": "002"}])
```
4. Reload the webpage (1 result must be rendered), Do:
```
> db.Catalog.updateOne({ "_id": ObjectId("62424350f1e607a8632f691f") }, {$set: { "Name": "Alcachofa", "Sku": "002", "IsEnabled": true }})
```
5. Reload the webpage to see 2 results listed

Now, you can stop/remove/start docker-compose with `docker-compose down && docker-compose up --build` command. You'll notice that heading to http://localhost:5002 gets you 2 listed results. This is because MongoDB container is using a persisted volume outside its boundaries. NOTE: this we'll later be the basis for local/manual backup.

