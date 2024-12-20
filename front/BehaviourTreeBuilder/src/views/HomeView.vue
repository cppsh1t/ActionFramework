<template>
  <GraphView class="w-full h-full" />
</template>

<script setup lang="ts">
import GraphView from "@/components/GraphView.vue"
import jsonData from "@/resources/node-definitions.json"
import type { NodeDefinition } from "@/utils/type"

const definitions: NodeDefinition[] =
  jsonData.definitions as unknown as NodeDefinition[]

console.log(definitions)
</script>
