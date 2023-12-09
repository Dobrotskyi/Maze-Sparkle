using TMPro;
using UnityEngine;

public class SkillInfoPopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameField;
    [SerializeField] private TextMeshProUGUI _descriptionField;
    [SerializeField] private GameObject _body;

    public void ShowSkillInfo(string name, string description)
    {
        _body.SetActive(true);
        _nameField.text = name;
        _descriptionField.text = description;
    }

    private void OnEnable()
    {
        if (_body.activeSelf)
            _body.SetActive(false);
    }

    private void OnDisable()
    {
        _nameField.text = "Skill name";
        _descriptionField.text = "Skill description";
    }
}
