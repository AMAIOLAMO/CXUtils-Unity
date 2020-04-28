using UnityEngine;
using CXUtils.Evolutions.NeuralNetworks;
using CXUtils.Evolutions.GeneticAlgorithm;
using CXUtils.CodeUtils;
using UnityEngine.UI;

namespace CXUtils.Test
{
    public class TestingScript_NNGAExample : MonoBehaviour
    {
#pragma warning disable IDE0044

        CXNNetGeneAlgorithm CXNNGA;
        [SerializeField] private Transform berryPos = default;

        #region texts

        [SerializeField] Text timeLeftTxt = default;
        [SerializeField] Text generationCountText = default;
        [SerializeField] Text indexText = default;

        #endregion

        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotSpeed = 5f;
        [SerializeField] int currentNetIndex;

        private float currentTimeSinceStart;

        private void Start()
        {
            CXNNGA = new CXNNetGeneAlgorithm(10, 2, 2, 10, 2, .2f);
            currentNetIndex = 0;
            currentTimeSinceStart = 0;

            generationCountText.text = $"Generation : {CXNNGA.Generation.ToString()}";
            indexText.text = $"#Index : {currentNetIndex + 1}";
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                ResetWhole();

            if (currentNetIndex < CXNNGA.Population.Length)
            {
                CXNeuralNetwork currentNetwork = CXNNGA.Population[currentNetIndex];
                bool isval = !(currentNetwork.fitness < 1);

                if (currentTimeSinceStart > 15 && !isval)
                    ResetWhole();

                else if (currentTimeSinceStart > 50)
                    ResetWhole();
                else
                {
                    currentTimeSinceStart += Time.deltaTime;
                    timeLeftTxt.text = $"TimeLeft: { (isval ? 50 - currentTimeSinceStart : 15 - currentTimeSinceStart).ToString("0") }";
                }
            }
        }

        private void FixedUpdate()
        {
            if (currentNetIndex < CXNNGA.Population.Length)
            {
                CXNeuralNetwork currentNetwork = CXNNGA.Population[currentNetIndex];

                Vector2 toBerryPos = GetToBerryPos();

                float[] output = currentNetwork.RunNetworkTanh(new float[] { toBerryPos.x, toBerryPos.y });

                float turnSpeed = output[0];
                float moveSpeed = MathUtils.Sigmoid_1(output[1]);

                TurnPlayer(turnSpeed);
                MovePlayer(moveSpeed);
            }
        }

        #region Script Utils

        private void MovePlayer(float value) =>
            transform.position += transform.up * value * Time.deltaTime * moveSpeed;

        private void TurnPlayer(float value) =>
            transform.eulerAngles += new Vector3(0, 0, value * rotSpeed);

        private Vector2 GetToBerryPos() =>
            berryPos.position - transform.position;

        private void ResetWhole()
        {
            currentNetIndex++;

            if (currentNetIndex == CXNNGA.Population.Length)
            {
                currentNetIndex = 0;

                CXNNGA.NewGeneration();
                generationCountText.text = $"Generation: {CXNNGA.Generation.ToString()}";
                print("New generation!");
            }

            currentTimeSinceStart = 0;
            transform.position = Vector2.zero;
            transform.rotation = Quaternion.identity;
            indexText.text = $"#Index : {currentNetIndex + 1}";
            timeLeftTxt.text = $"TimeLeft: 0";
        }

        #endregion

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Berry"))
            {
                CXNNGA.Population[currentNetIndex].fitness += 1;
                print($"pop got new fitness of: {CXNNGA.Population[currentNetIndex].fitness}");
                collision.transform.position = Random.insideUnitSphere * 3.5f;
            }

            if (collision.collider.CompareTag("Blocks"))
            {
                CXNNGA.Population[currentNetIndex].fitness--;
                ResetWhole();
            }
        }
    }
}