# Bag o' Loot API

Based on the Bag o' Loot command line application that you have worked on during orientation, this API will expose two resources for client developers to consume for other application they want to write.

1. The **Child** resource at the URI of `http://localhost:5000/api/child`
2. The **Toy** resource at the URI of `http://localhost:5000/api/toy`

You can use this as a baseline resource when building your own Web API.

## How to Run

After cloning the repo, restore dependencies and apply the migration.

```
dotnet restore
dotnet ef database update
```

Then run the project, which will seed the database with a few children, a few toys assign to those children, some reindeer, and then some relationships between the children and their favorite reindeer.

## Viewing seeded data

To view children 

```
curl -s -X GET -D - http://localhost:5000/api/child
```

To view toys 

```
curl -s -X GET -D - http://localhost:5000/api/toy
```

To view reindeer 

```
curl -s -X GET -D - http://localhost:5000/api/reindeer
```

