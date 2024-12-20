<template>
  <div ref="graphContainer" @mousedown="handleRightClick"></div>
</template>
<script setup lang="ts">
import { Graph } from "@antv/x6"
import { onMounted, ref } from "vue"
import { register } from "@antv/x6-vue-shape"
import ProgressNode from "./ProgressNode.vue"

register({
  shape: "test-node",
  width: 100,
  height: 100,
  component: ProgressNode,
})

const graphContainer = ref()

const data = {
  nodes: [
    {
      id: "node1",
      shape: "rect",
      x: 40,
      y: 40,
      width: 100,
      height: 40,
      label: "hello",
      attrs: {
        // body 是选择器名称，选中的是 rect 元素
        body: {
          stroke: "#8f8f8f",
          strokeWidth: 1,
          fill: "#fff",
          rx: 6,
          ry: 6,
        },
      },
    },
    {
      id: "node2",
      shape: "rect",
      x: 160,
      y: 180,
      width: 100,
      height: 40,
      label: "world",
      attrs: {
        body: {
          stroke: "#8f8f8f",
          strokeWidth: 1,
          fill: "#fff",
          rx: 6,
          ry: 6,
        },
      },
    },
  ],
  edges: [
    {
      shape: "edge",
      source: "node1",
      target: "node2",
      label: "x6",
      attrs: {
        // line 是选择器名称，选中的边的 path 元素
        line: {
          stroke: "#8f8f8f",
          strokeWidth: 1,
        },
      },
    },
  ],
}

function handleRightClick(e: MouseEvent) {
  if (e.button == 2) {
    e.preventDefault()
    console.log("right click")
  }
}

onMounted(() => {
  const graph = new Graph({
    container: graphContainer.value,
    width: graphContainer.value.width,
    height: graphContainer.value.height,
    background: {
      color: "#F2F7FA",
    },
  })

  graph.fromJSON(data) // 渲染元素
  graph.addNode({
    shape: "test-node",
    x: 100,
    y: 60,
  })
  graph.centerContent() // 居中显示
})
</script>
