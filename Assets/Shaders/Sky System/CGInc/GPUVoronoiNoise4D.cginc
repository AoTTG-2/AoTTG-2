

uniform float _Frequency, _Jitter;

//1/7
#define K 0.142857142857
//3/7
#define Ko 0.428571428571

float4 mod(float4 x, float y) { return x - y * floor(x/y); }
float3 mod(float3 x, float y) { return x - y * floor(x/y); }

// Permutation polynomial: (34x^2 + x) mod 289
float3 Permutation(float3 x) 
{
  return mod((34.0 * x + 1.0) * x, 289.0);
}

float2 inoise(float4 P, float jitter)
{			
	float4 Pi = mod(floor(P), 289.0);
 	float4 Pf = frac(P);
	float3 oi = float3(-1.0, 0.0, 1.0);
	float3 of = float3(-0.5, 0.5, 1.5);
	float3 px = Permutation(Pi.x + oi);
	float3 py = Permutation(Pi.y + oi);
	float3 pz = Permutation(Pi.z + oi);

	float3 p, ox, oy, oz, ow, dx, dy, dz, dw, d;
	float2 F = 1e6;
	int i, j, k, n;

	for(i = 0; i < 3; i++)
	{
		for(j = 0; j < 3; j++)
		{
			for(k = 0; k < 3; k++)
			{
				p = Permutation(px[i] + py[j] + pz[k] + Pi.w + oi); // pijk1, pijk2, pijk3
	
				ox = frac(p*K) - Ko;
				oy = mod(floor(p*K),7.0)*K - Ko;
				
				p = Permutation(p);
				
				oz = frac(p*K) - Ko;
				ow = mod(floor(p*K),7.0)*K - Ko;
			
				dx = Pf.x - of[i] + jitter*ox;
				dy = Pf.y - of[j] + jitter*oy;
				dz = Pf.z - of[k] + jitter*oz;
				dw = Pf.w - of + jitter*ow;
				
				d = dx * dx + dy * dy + dz * dz + dw * dw; // dijk1, dijk2 and dijk3, squared
				
				//Find the lowest and second lowest distances
				for(n = 0; n < 3; n++)
				{
					if(d[n] < F[0])
					{
						F[1] = F[0];
						F[0] = d[n];
					}
					else if(d[n] < F[1])
					{
						F[1] = d[n];
					}
				}
			}
		}
	}
	
	return F;
}

// fractal sum, range -1.0 - 1.0
float fBm_F0(float4 p, int octaves)
{
	float sum = 0;	
	float2 F = inoise(p * _Frequency, _Jitter) * 0.5;
		
	sum += 0.1 + sqrt(F[0]);
		
	return sum;
}

//float fBm_F1_F0(float4 p, int octaves)
//{
//	float freq = _Frequency, amp = 0.5;
//	float sum = 0;	
//	for(int i = 0; i < octaves; i++) 
//	{
//		float2 F = inoise(p * freq, _Jitter) * amp;
//		
//		sum += 0.1 + sqrt(F[1]) - sqrt(F[0]);
//		
//		freq *= _Lacunarity;
//		amp *= _Gain;
//	}
//	return sum;
//}