using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    
    public static BuildingSystem Instance;

    public GridLayout GridLayout;

    private Grid _grid;

    [SerializeField]
    private Tilemap _mainTileMap;

    public List<GameObject> GameObjects;

    private Placeable _placeable;

    private int _currentPrefabIndex = 0;

    private int _rotatie = 0;

    private void Awake()
    {
        Instance = this;
        _grid = GridLayout.gameObject.GetComponent<Grid>(); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_currentPrefabIndex == GameObjects.Count - 1)
                _currentPrefabIndex = 0;
            else
                _currentPrefabIndex++;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_rotatie == 360)
                _rotatie = 0;
            else         
                _rotatie += 90;
        }

        if (Input.GetMouseButtonDown(1))
            DeleteObject();

        if (Input.GetMouseButtonDown(0))
            InitializeWithObject(GameObjects[_currentPrefabIndex]);   
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
            return raycastHit.point;
        else
            return Vector3.zero;
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = GridLayout.WorldToCell(position);

        position = _grid.GetCellCenterWorld(cellPos);

        return position;
    }

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 pos = SnapCoordinateToGrid(GetMouseWorldPosition());

        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
        obj.transform.rotation = Quaternion.Euler(0, _rotatie, 0);
        _placeable = obj.GetComponent<Placeable>();
    }

    private void DeleteObject() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
            if (raycastHit.transform.GetComponent<Placeable>() != null)
                Destroy(raycastHit.transform.parent.gameObject);     
    }
}
