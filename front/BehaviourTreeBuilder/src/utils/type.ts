export type NodeDefinition = {
  nodeName: string
  fields: string[]
  type: "composite" | "action" | "decorator"
}
