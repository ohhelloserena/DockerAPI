# DockerAPI

## Database

The tree data is stored in MongoDB Atlas to allow for data persistance.

This is a diagram of the data:

![diagram](https://github.com/ohhelloserena/DockerAPI/blob/master/TreeDiagram.png)

## Assumptions/Notes

- The prompt indicates that each node should have the height, but defined the height of the root as. 0 would be the depth of the root node. For my implementation, I provided the depth of the node, where depth is defined as the number of edges from the node to the tree's root node.
A root node will have a depth of 0.
* The connection string is currently stored in a services class, however it should be moved elsewhere or encrypted for security.
