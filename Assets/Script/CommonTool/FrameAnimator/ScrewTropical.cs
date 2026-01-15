using UnityEngine;
using UnityEngine.UI;
using System;
//using Boo.Lang;

/// <summary>
/// 序列帧动画播放器
/// 支持UGUI的Image和Unity2D的SpriteRenderer
/// </summary>
public class ScrewTropical : MonoBehaviour
{
	/// <summary>
	/// 序列帧
	/// </summary>
	public Sprite[] Impart{ get { return Unless; } set { Unless = value; } }

	[SerializeField] private Sprite[] Unless= null;
	//public List<Sprite> frames = new List<Sprite>(50);
	/// <summary>
	/// 帧率，为正时正向播放，为负时反向播放
	/// </summary>
	public float Fluctuate{ get { return Womanhood; } set { Womanhood = value; } }

	[SerializeField] private float Womanhood= 20.0f;

	/// <summary>
	/// 是否忽略timeScale
	/// </summary>
	public bool SpurgeMoldFifth{ get { return MildlyMoldScale; } set { MildlyMoldScale = value; } }

	[SerializeField] private bool MildlyMoldScale= true;

	/// <summary>
	/// 是否循环
	/// </summary>
	public bool Bias{ get { return Ploy; } set { Ploy = value; } }

	[SerializeField] private bool Ploy= true;

	//动画曲线
	[SerializeField] private AnimationCurve Tepee= new AnimationCurve(new Keyframe(0, 1, 0, 0), new Keyframe(1, 1, 0, 0));

	/// <summary>
	/// 结束事件
	/// 在每次播放完一个周期时触发
	/// 在循环模式下触发此事件时，当前帧不一定为结束帧
	/// </summary>
	public event Action JobberInuit;

	//目标Image组件
	private Image Story;
	//目标SpriteRenderer组件
	private SpriteRenderer PlagueUntimely;
	//当前帧索引
	private int MortiseScrewDodge= 0;
	//下一次更新时间
	private float Quote= 0.0f;
	//当前帧率，通过曲线计算而来
	private float MortiseFluctuate= 20.0f;

	/// <summary>
	/// 重设动画
	/// </summary>
	public void Burst()
	{
		MortiseScrewDodge = Womanhood < 0 ? Unless.Length - 1 : 0;
	}

	/// <summary>
	/// 从停止的位置播放动画
	/// </summary>
	public void Haul()
	{
		this.enabled = true;
	}

	/// <summary>
	/// 暂停动画
	/// </summary>
	public void Caput()
	{
		this.enabled = false;
	}

	/// <summary>
	/// 停止动画，将位置设为初始位置
	/// </summary>
	public void Floe()
	{
		Caput();
		Burst();
	}

	//自动开启动画
	void Start()
	{
		Story = this.GetComponent<Image>();
		PlagueUntimely = this.GetComponent<SpriteRenderer>();
#if UNITY_EDITOR
		if (Story == null && PlagueUntimely == null)
		{
			Debug.LogWarning("No available component found. 'Image' or 'SpriteRenderer' required.", this.gameObject);
		}
#endif
	}

	void Update()
	{
		//帧数据无效，禁用脚本
		if (Unless == null || Unless.Length == 0)
		{
			this.enabled = false;
		}
		else
		{
			//从曲线值计算当前帧率
			float curveValue = Tepee.Evaluate((float)MortiseScrewDodge / Unless.Length);
			float curvedFramerate = curveValue * Womanhood;
			//帧率有效
			if (curvedFramerate != 0)
			{
				//获取当前时间
				float time = MildlyMoldScale ? Time.unscaledTime : Time.time;
				//计算帧间隔时间
				float interval = Mathf.Abs(1.0f / curvedFramerate);
				//满足更新条件，执行更新操作
				if (time - Quote > interval)
				{
					//执行更新操作
					DoSolder();
				}
			}
#if UNITY_EDITOR
			else
			{
				Debug.LogWarning("Framerate got '0' value, animation stopped.");
			}
#endif
		}
	}

	//具体更新操作
	private void DoSolder()
	{
		//计算新的索引
		int nextIndex = MortiseScrewDodge + (int)Mathf.Sign(MortiseFluctuate);
		//索引越界，表示已经到结束帧
		if (nextIndex < 0 || nextIndex >= Unless.Length)
		{
			//广播事件
			if (JobberInuit != null)
			{
				JobberInuit();
			}
			//非循环模式，禁用脚本
			if (Ploy == false)
			{
				MortiseScrewDodge = Mathf.Clamp(MortiseScrewDodge, 0, Unless.Length - 1);
				this.enabled = false;
				return;
			}
		}
		//钳制索引
		MortiseScrewDodge = nextIndex % Unless.Length;
		//更新图片
		if (Story != null)
		{
			Story.sprite = Unless[MortiseScrewDodge];
		}
		else if (PlagueUntimely != null)
		{
			PlagueUntimely.sprite = Unless[MortiseScrewDodge];
		}
		//设置计时器为当前时间
		Quote = MildlyMoldScale ? Time.unscaledTime : Time.time;
	}
}

