using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    // VR 컨트롤러나 레이저로 오브젝트에 포인터가 닿았을 때 발생
    #region Hover Event

    // 해당 물체를 처음으로 어떤 인터렉트가 지목 했을 때, 처음 Hover 시작
    public void OnFirstHoverEntered()
    {
        Debug.Log($"{gameObject.name} > OnFirstHoverEntered");
    }

    // 누군가(혹은 여러명)가 이 물체를 지목할 때, 지목 진입
    public void OnHoverEntered()
    {
        Debug.Log($"{gameObject.name} > OnHoverEntered");
    }

    // 어떤 인터랙터가 지목을 끝낼 때, 지목 해제
    public void OnHoverExited()
    {
        Debug.Log($"{gameObject.name} > OnHoverExited");
    }

    // 마지막 인터랙터가 지목을 끝내서 아무도 지목하지 않는 상태가 되었을 때, 완전 지목 해제
    public void OnLastHoverExited()
    {
        Debug.Log($"{gameObject.name} > OnLastHoverExited");
    }

    #endregion

    // VR 컨트롤러로 잡거나 클릭하는 등 직접 선택했을 때 발생
    #region Select Event

    // 오브젝트가 처음으로 선택됨
    public void OnFirstSelectEntered()
    {
        Debug.Log($"{gameObject.name} > OnFirstSelectEntered");
    }

    // 선택될 때마다
    public void OnSelectEntered()
    {
        Debug.Log($"{gameObject.name} > OnSelectEntered");
    }

    // 인터랙터가 선택을 놓았을 때
    public void OnSelectExited()
    {
        Debug.Log($"{gameObject.name} > OnSelectExited");
    }

    // 아무도 선택하지 않는 상태가 되었을 때
    public void OnLastSelectExited()
    {
        Debug.Log($"{gameObject.name} > OnLastSelectExited");
    }

    #endregion

    public void OnFirstFocusEntered()
    {
        Debug.Log($"{gameObject.name} > OnFirstFocusEntered");
    }

    public void OnLastFocusEixted()
    {
        Debug.Log($"{gameObject.name} > OnLastFocusEixted");
    }

    public void OnFocusEntered()
    {
        Debug.Log($"{gameObject.name} > OnFocusEntered");
    }

    public void OnFocusEixted()
    {
        Debug.Log($"{gameObject.name} > OnFocusEixted");
    }

    public void OnActivateEntered()
    {
        Debug.Log($"{gameObject.name} > OnActivateEntered");
    }

    public void OnActivateEixted()
    {
        Debug.Log($"{gameObject.name} > OnActivateEixted");
    }
}
