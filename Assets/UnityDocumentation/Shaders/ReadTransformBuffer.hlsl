
#ifndef VOXEL_MESH_INFO
#define VOXEL_MESH_INFO


StructuredBuffer<float4x4> _TransformBuffer;
void GetPositionMatrix_float(float i, out float4x4 v)
{


    v = _TransformBuffer[i];
   
}
#endif