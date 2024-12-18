<template>
  <div ref="graphContainer"></div>
</template>
<script setup lang="ts">
import { Graph } from "@antv/x6"
import { onMounted, ref } from "vue"

const graphContainer = ref()

onMounted(() => {
  const graph = new Graph({
    container: graphContainer.value,
    width: 1366,
    height: 768,
    background: {
      color: "#F2F7FA",
    },
  })
})
</script>
