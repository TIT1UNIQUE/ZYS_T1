using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.game.VIC.ui.Message.MessageComposer
{
    /// <summary>
    /// parts
    /// 左上角有一个提示区PromptPanel
    ///     一段纯文字、词条
    ///     显示对当前对话的客户画像描述
    ///     前端销售填写的
    /// 主体上方消息显示区MessageDisplayerPanel
    ///     单个消息
    ///         title content profile date
    ///     可以显示多条消息
    ///         自己的消息，右对齐，头像在右边
    ///         对方的消息，左对齐，头像在左边
    ///     支持消息滚动功能
    ///         自动滚动
    ///         手动滚动（鼠标滚轮）
    /// 主体下方显示编辑中的消息 MessageComposerPanel
    ///     拼字游戏完型填空的风格
    ///     有预置的文字和空缺的文字框
    ///         因为空缺的文字长度不同
    ///         所以option（可以用的文字选项）是一样的
    ///         如果要填非常长的东西，比如超过30个字母，可以拆成2~3段option
    ///         内容支持：
    ///             文字
    ///             richtext tag
    ///             默认emoji
    ///             题目用6个下划线表示______
    ///         内容字符实例：
    ///             <color=#AA00FF><i>wq</i></color>ha______h<size=200%>hs</size>eqwe<color=#FFAAAA>wq</color>Your best enemy!!!!-<sprite index=0><size=200%><sprite index=1></size><sprite index=2><sprite index=3><sprite index=4><sprite index=5><sprite index=6><sprite index=7><sprite index=8><sprite index=9><sprite index=10><sprite index=11><sprite index=12>
    /// 左上角显示好感度
    ///     建议改为公司SOP进度显示
    ///         初步对接
    ///         项目传达
    ///         合同修订
    ///         签约，录入系统
    ///         缴费
    /// 
    /// 
    /// 
    /// 
    /// 
    /// </summary>
    public class MessageComposerSystem : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}