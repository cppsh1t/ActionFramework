<template>
  <div class="p-[6px] rounded-xl bg-white">
    <el-progress type="dashboard" :percentage="percentage" :width="80">
      <template #default="{ percentage }">
        <span class="percentage-value">{{ percentage }}%</span>
      </template>
    </el-progress>
  </div>
</template>

<script lang="ts" setup>
import type { Node } from "@antv/x6"
import { inject, onMounted, ref } from "vue"

const getNode = inject("getNode") as () => Node

const percentage = ref(80)

onMounted(() => {
  const node = getNode()
  console.log(node)
})
</script>
